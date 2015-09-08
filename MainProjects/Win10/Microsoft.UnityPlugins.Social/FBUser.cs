using Facebook;
using System;
using System.Collections.Generic;

#if NETFX_CORE
using Facebook.Client;
#endif


namespace Microsoft.UnityPlugins
{
    /// <summary>
    /// Provides a strongly-typed representation of a Facebook User as defined by the Graph API.
    /// </summary>
    /// <remarks>
    /// The GraphUser class represents the most commonly used properties of a Facebook User object.
    /// </remarks>
    public class FBUser
    {
        /// <summary>
        /// Initializes a new instance of the FBUser class.
        /// </summary>
        public FBUser()
        {
        }

#if NETFX_CORE
        /// <summary>
        /// Initializes a new instance of the FBUser class from a GraphUser
        /// </summary>
        public FBUser(GraphUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            this.Id = user.Id;
            this.Name = user.Name;
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.MiddleName = user.MiddleName;
            this.LastName = user.LastName;
            this.Birthday = user.Birthday;
            if (user.Location == null)
                this.Location = null;
            else
                this.Location = new FBLocation(user.Location);
            this.Link = user.Link;
            this.ProfilePictureUrl = user.ProfilePictureUrl;
        }
#endif

        /// <summary>
        /// Gets or sets the ID of the user.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Facebook username of the user.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the middle name of the user.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the birthday of the user.
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// Gets or sets the Facebook URL of the user.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the current city of the user.
        /// </summary>
        public FBLocation Location { get; set; }

        /// <summary>
        /// Gets or sets the URL of the user's profile picture.
        /// </summary>
        public Uri ProfilePictureUrl { get; set; }
    }
}