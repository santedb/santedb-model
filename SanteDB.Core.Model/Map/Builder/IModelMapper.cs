using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Map.Builder
{

    /// <summary>
    /// Represents a model mapper instance
    /// </summary>
    public interface IModelMapper
    {

        /// <summary>
        /// Source type
        /// </summary>
        Type SourceType { get; }

        /// <summary>
        /// Target type
        /// </summary>
        Type TargetType { get; }

        /// <summary>
        /// Map from source to target
        /// </summary>
        Object MapToTarget(Object source);

        /// <summary>
        /// Map from target to source
        /// </summary>
        Object MapToSource(Object target);

    }

    /// <summary>
    /// Model mapper
    /// </summary>
    public interface IModelMapper<TSource, TTarget> : IModelMapper
        where TSource: new ()
        where TTarget: new()
    {

        /// <summary>
        /// Maps <paramref name="source"/> from <typeparamref name="TSource"/> to <typeparamref name="TTarget"/>
        /// </summary>
        /// <param name="source"></param>
        TTarget MapToTarget(TSource source);

        /// <summary>
        /// Performs a reverse map on <paramref name="target"/> to <typeparamref name="TTarget"/> from <typeparamref name="TSource"/>
        /// </summary>
        TSource MapToSource(TTarget target);

    }
}
