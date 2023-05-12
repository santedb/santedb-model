namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Implementers of this interface claim that they can store and convey external identification keys for their object
    /// </summary>
    public interface IHasExternalKey
    {

        /// <summary>
        /// Gets or sets the external key for the object
        /// </summary>
        /// <remarks>Sometimes, when communicating with an external communications another system needs to 
        /// refer to this by a particular key</remarks>
        string ExternalKey { get; set; }

    }
}
