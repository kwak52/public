﻿// http://fssnip.net/7Z/title/Sliding-window-for-Observable

module Dsu.Common.Utilities.Observable
open System

/// Returns an observable that yields sliding windows of 
/// containing elements drawn from the input observable. 
/// Each window is returned as a fresh array.
let windowed (count:int) (source:IObservable<_>) =
    { new IObservable<_> with
        member x.Subscribe(observer) =
            // Start an agent that remembers partial windows of length 
            // smaller than the count (new agent for every observer)
            let agent = MailboxProcessor.Start(fun agent ->
                // The parameter 'lists' contains partial lists and their lengths
                let rec loop lists =
                    async { 
                        // Receive the next value
                        let! value = agent.Receive()

                        // Add new empty list and then the new element to all lists.
                        // Then split the lists into 'full' that should be sent
                        // to the observer and 'partial' which need more elements.
                        let full, partial =
                            ((0, []) :: lists)
                            |> List.map (fun (length, l) -> length + 1, value::l)
                            |> List.partition (fun (length, l) -> length = count)
              
                        // Send all full lists to the observer (as arrays)
                        for (_, l) in full do
                            observer.OnNext(l |> Array.ofSeq |> Array.rev) 
                        // Continue looping with incomplete lists
                        return! loop partial }

                // Start with an empty list of partial lists
                loop [])

            // Send incoming values to the agent
            source.Subscribe(agent.Post) }