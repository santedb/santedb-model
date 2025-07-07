using SanteDB.Core.i18n;
using SanteDB.Core.Model.Acts;
using SanteDB.Core.Model.DataTypes;
using SanteDB.Core.Model.Entities;
using SanteDB.Core.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;

namespace SanteDB.Core.Model
{
    /// <summary>
    /// Collection utilities
    /// </summary>
    public static class CollectionUtils
    {

        private class DependencyNode
        {

            public DependencyNode(IdentifiedData identifiedData, IEnumerable<DependencyNode> containerTree)
            {
                this.NodeKey = identifiedData.Key.Value;
                this.OutboundDependencies = new HashSet<Guid>();
                this.InboundDependencies = new HashSet<DependencyNode>();
                this.Initialize(identifiedData, containerTree);
            }


            private void Initialize(IdentifiedData identifiedData, IEnumerable<DependencyNode> containerTree) { 
                Guid?[] dependencies = null;
                switch(identifiedData)
                {
                    case Entity entity:
                        dependencies = entity.Relationships?.Select(r => r.TargetEntityKey == identifiedData.Key ? r.SourceEntityKey : r.TargetEntityKey).ToArray() ?? new Guid?[0];
                        dependencies = entity.Participations != null ? dependencies.Union(entity.Participations.Select(p => p.ActKey)).ToArray() : dependencies;
                        if (entity.CreationActKey.HasValue)
                        {
                            dependencies = dependencies.Union(new Guid?[] { entity.CreationActKey }).ToArray();
                        }
                        break;
                    case Act act:
                        dependencies = act.Relationships?.Select(r => r.TargetActKey == identifiedData.Key ? r.SourceEntityKey : r.TargetActKey).ToArray() ?? new Guid?[0];
                        dependencies = act.Participations != null ? dependencies.Union(act.Participations.Select(p => p.PlayerEntityKey)).ToArray() : dependencies;
                        break;
                    case Concept concept:
                        dependencies = concept.Relationships?.Select(r => r.TargetConceptKey == identifiedData.Key ? r.SourceEntityKey : r.TargetConceptKey).ToArray();
                        break;
                    case ITargetedAssociation ta:
                        dependencies = new Guid?[] { ta.TargetEntityKey, ta.SourceEntityKey };
                        break;
                    default:
                        return;
                }

                foreach(var itm in dependencies.Where(o=>o.HasValue && o != this.NodeKey).Select(o=>o.Value).Distinct()) this.OutboundDependencies.Add(itm);

                var inboundDependencies = new HashSet<DependencyNode>();
                // Look for inbound dependencies on our tree that point to me and add myself as an inbound dependency
                foreach (var node in containerTree.Where(o => o.OutboundDependencies.Contains(identifiedData.Key.Value)))
                {
                    if (!this.InboundDependencies.Contains(node))
                    {
                        this.InboundDependencies.Add(node);
                    }
                }
                // Look for output dependencies in our node and instruct those nodes that we're an inbound dependency on them
                foreach(var node in containerTree.Where(o=> dependencies.Contains(o.NodeKey)))
                {
                    if(!node.InboundDependencies.Contains(this))
                    {
                        node.InboundDependencies.Add(this);
                    }
                }

            }

            /// <summary>
            /// Node key
            /// </summary>
            public Guid NodeKey { get; }

            /// <summary>
            /// Outbound dependencies
            /// </summary>
            public ICollection<Guid> OutboundDependencies { get; }

            /// <summary>
            /// Inbound dependencies
            /// </summary>
            public ICollection<DependencyNode> InboundDependencies { get; }

            /// <inheritdoc/>
            public override int GetHashCode() => this.NodeKey.GetHashCode();

            /// <inheritdoc/>
            public override string ToString() => this.NodeKey.ToString();
        }

        /// <summary>
        /// Reorganize the collection for insert
        /// </summary>
        public static IEnumerable<IdentifiedData> ReorganizeForInsert(this IEnumerable<IdentifiedData> collection)
        {
            var collectionDictionary = collection.ToDictionaryIgnoringDuplicates(o => o.Key ?? Guid.NewGuid(), o => o);
            var dependencyTree = new HashSet<DependencyNode>();
            foreach (var itm in collection.Where(n => n.Key.HasValue))
            {
                dependencyTree.Add(new DependencyNode(itm, dependencyTree));
            }
            
            // Organize the tree 
            while(dependencyTree.Any())
            {
                // Find the next leaf in the tree
                var node = dependencyTree.FirstOrDefault(o => !o.OutboundDependencies.Any(b => collectionDictionary.ContainsKey(b))) ?? // If this is null there is a circular dependency
                     dependencyTree.FirstOrDefault(o=>collectionDictionary.TryGetValue(o.NodeKey, out var v) && v.BatchOperation == BatchOperationType.Update); // We can also attempt to exclude updates since they already exist
                if(node == null)
                {
                    throw new InvalidOperationException(ErrorMessages.DATA_CIRCULAR_REFERENCE);
                }
                if(collectionDictionary.TryGetValue(node.NodeKey, out var rv))
                {
                    yield return rv;
                    collectionDictionary.Remove(node.NodeKey);
                }
                // Remove node from tree
                dependencyTree.Remove(node);
                foreach(var dn in node.InboundDependencies)
                {
                   dn.OutboundDependencies.Remove(node.NodeKey);
                }
            }

            // If there are any items in the original collection left return them
            foreach(var itm in collectionDictionary)
            {
                yield return itm.Value;
            }

        }
    }
}
