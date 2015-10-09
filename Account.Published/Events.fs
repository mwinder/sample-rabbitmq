namespace Account.Published

open System;

type UserRegistered(name : string, occuredAt : DateTime) =
    member this.Name = name;
    member this.OccuredAt = occuredAt;

type TrialStarted = {
    FirstName: string;
    Surname: string;
    Email: string;
    Organisation: string;
    OccuredAt: DateTime;
}
