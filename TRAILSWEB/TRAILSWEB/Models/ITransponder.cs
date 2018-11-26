using System;

namespace TRAILSWEB.Models
{
    interface ITransponder
    {
        string TransponderNumber { get; set; }
        int TransponderType { get; set; }
        string ShortDescription { get; set; }
    }
}
