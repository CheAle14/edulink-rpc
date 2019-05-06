using System;
using System.Collections.Generic;

namespace EduLinkRPC.Classes
{
    public interface IHomework
    {
        int Id { get; }
        string Activity { get; }
        List<IHwkUser> AppliesTo { get; set; }
        List<HwkAttachment> Attachments { get; }
        DateTime AvailableDate { get; }
        string AvailableText { get; }
        bool Completed { get; }
        bool Current { get; }
        int DaysAvailable { get; }
        int DaysUntilDue { get; }
        string Description { get; }
        DateTime DueDate { get; }
        string DueText { get; }
        int OwnerId { get; }
        string SetBy { get; }
        string Status { get; }
        string Subject { get; }
        string UserType { get; }
    }
}