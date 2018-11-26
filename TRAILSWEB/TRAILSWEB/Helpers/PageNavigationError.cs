using System;

namespace TRAILSWEB.Helpers
{
    public class PageNavigationError
    {
        public bool HasError { get; set; }
        public bool HasModal { get; set; }
        public string[] ModalIds { get; set; }
        public string[] Errors { get; set; }
    }
}