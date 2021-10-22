﻿namespace PnP.Core.Admin
{
    /// <summary>
    /// Support class to provide all the constants for the admin operations
    /// </summary>
    internal static class PnPAdminConstants
    {
        // site templates

        /// <summary>
        /// Web template of a communication site
        /// </summary>
        internal const string CommunicationSiteTemplate = "SITEPAGEPUBLISHING#0";

        /// <summary>
        /// Web template of a group connected team site
        /// </summary>
        internal const string TeamSiteTemplate = "GROUP#0";

        /// <summary>
        /// Web template of a team site without a group
        /// </summary>
        internal const string TeamSiteWithoutGroupTemplate = "STS#3";

        // CSOM objects

        /// <summary>
        /// Identifier for the CSOM Tenant object
        /// </summary>
        internal const string CsomTenantObject = "{268004ae-ef6b-4e9b-8425-127220d84719}";

    }
}