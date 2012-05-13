// Guids.cs
// MUST match guids.h

using System;

namespace AnkhVersionOneConnector
{
    static class GuidList
    {
        public const string guidAnkhVersionOneConnectorPkgString = "7976548b-4eff-44a2-895d-584f62d207d0";
        public const string guidAnkhVersionOneConnectorCmdSetString = "6b6b88a9-1ac9-4c80-90c1-367ee826fe75";
        public const string guidAnkhVersionOneConnectorConnectorString = "1c03bb98-6b18-41b9-9348-ebafdb6f14aa";

        public static readonly Guid guidAnkhVersionOneConnectorCmdSet = new Guid(guidAnkhVersionOneConnectorCmdSetString);
    };
}