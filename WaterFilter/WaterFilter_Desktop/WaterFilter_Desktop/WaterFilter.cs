using Microsoft.WindowsAzure.MobileServices;
using System;

class WaterFilter
{
    public string Id { get; set; }
    public Boolean sensor_1 { get; set; } = true;
    public Boolean sensor_2 { get; set; } = true;
    public Boolean sensor_3 { get; set; } = true;
    public DateTimeOffset? last1_1 { get; set; }
    public DateTimeOffset? last1_2 { get; set; }
    public DateTimeOffset? last1_3 { get; set; }
    public DateTimeOffset? last0_1 { get; set; }
    public DateTimeOffset? last0_2 { get; set; }
    public DateTimeOffset? last0_3 { get; set; }
    [CreatedAt]
    public DateTimeOffset? CreatedAt { get; set; }
}