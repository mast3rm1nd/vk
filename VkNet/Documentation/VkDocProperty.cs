﻿namespace VkNet.Documentation
{
    using System.Diagnostics;

    [DebuggerDisplay("Name = {ShortName} - {Summary}")]
    internal class VkDocProperty
    {
        public VkDocType Type { get; set; }
        public string FullName { get; set; }
        public string ShortName { get { return VkDocParser.GetShortName(FullName); } }
        public string Summary { get; set; }
    }
}