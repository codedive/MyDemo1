using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URFX.Data.Enums
{
    public enum URFXPaymentMode
    {
        Offline,
        Online
    }
    public enum URFXPaymentType
    {
        PlanPayment,
        JobPayment,
        JobAdditionalPayment
    }
    public enum ServiceProviderType
    {
        Corporate,
        Individual

    }
    public enum JobStatus
    {
        New,
        Rejected,
        Current,
        Completed

    }
    public enum EmployeeStatus
    {
        Available,
        Working

    }
    public enum ComplaintStatus
    {
        Open,
        Close

    }
    public enum RegistrationType
    {
        Simple,
        Facebook,
        Google,
        Twitter

    }
}
