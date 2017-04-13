
[<Measure>] type km
[<Measure>] type h
[<Measure>] type percent

///  FSharp.PowerPack.dll library 에 unit of measure 에 대한 표준화 셋 있다고 함.  pp.374

let length = 9.0<km>;;

length * length;;


let distanceInTwoHours(speed:float<km/h>) =
    speed * 2.0<h>;;

distanceInTwoHours(30.0<km/h>);;




let coef = 33.0<percent>

50.0<km> * coef / 100.0<percent>;;





