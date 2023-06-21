using System;
using System.Collections.Generic;
using System.Web;
using vb.Data;
public class Utility
{
    public string GetTextEnum(int Value)
    {
        string stringValue = Enum.GetName(typeof(Weekdays), Value);
        return stringValue;

    }

    public static int GetUserRoleId()
    {
        if (HttpContext.Current.Session != null && HttpContext.Current.Session["RoleId"] != null)
        {
            return Convert.ToInt32(HttpContext.Current.Session["RoleId"].ToString());
        }
        else
        {
            return 0;
        }
    }

}

public static class DateTimeExtensions
{

    public static DateTime FromFinancialYear(this DateTime dateTime)
    {
        return (dateTime.Month >= 4 ? dateTime : dateTime.AddYears(-1));
    }


    public static DateTime ToFinancialYear(this DateTime dateTime)
    {
        return (dateTime.Month >= 4 ? dateTime.AddYears(1) : dateTime);
    }
}

