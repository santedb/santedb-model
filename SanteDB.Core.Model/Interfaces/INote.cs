using System;
using System.Collections.Generic;
using System.Text;

namespace SanteDB.Core.Model.Interfaces
{
    /// <summary>
    /// Represents a note
    /// </summary>
    public interface INote : IIdentifiedResource
    {

        /// <summary>
        /// Gets or sets the author of the note
        /// </summary>
        Guid? AuthorKey { get; set; }

        /// <summary>
        /// Gets or sets the text of the note
        /// </summary>
        String Text { get; set; }

    }
}
