namespace Dsu.Driver.Paix

open System
open Dsu.Driver.PaixMotionControler
open Dsu.Driver

type DeviceType =
    | DeviceType_None = -1
    | NMC2_220S = 0
    | NMC2_420S = 1
    | NMC2_620S = 2
    | NMC2_820S = 3
    | NMC2_220 = 14
    | NMC2_420 = 15

/// pp.283
type DioType =
    | Dio_None = 0
    | Dio_16_16 = 1
    | Dio_32_32 = 2
    | Dio_48_48 = 3
    | Dio_64_64 = 4

type ExDioType =
    | ExDio_None = 0
    | ExDio_In16 = 1
    | ExDio_Out16 = 1

type MDioType =
    | MDio_None = 0
    | MDio_8_8 = 1

