﻿namespace Notes.Models;

public class Challenges : DomainObject
{
    public User AccountHolder { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public (double, double) StartLocation { get; set; }
    public (double, double) Endlocation { get; set; }
    public TimeSpan AvailabilityRange { get; set; }
    public double Duration { get; set; }
    public string Difficulty { get; set; }
    public double Rating { get; set; }
    public string ChallengeType { get; set; }
    public bool IsActive { get; set; }
    public string Category { get; set; }

}