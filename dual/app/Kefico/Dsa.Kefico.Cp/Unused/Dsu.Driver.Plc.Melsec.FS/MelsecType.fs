/// MITSUBISH Melsec PLC related type definitions
module Dsu.Driver.Plc.Melsec.Type


/// Melsec Q/A/FX Cpu types
type QnACpuType =
    | Q00 = 0x31
    | Q00J = 0x30
    | Q01 = 0x32
    | Q02H = 0x22
    | Q02H_A = 0x141
    | Q02PH = 0x45
    | Q02U = 0x83
    | Q03UD = 0x70
    | Q03UDE = 0x90
    | Q04UDEH = 0x91
    | Q04UDH = 0x71
    | Q06H = 0x23
    | Q06H_A = 0x142
    | Q06PH = 0x46
    | Q06UDEH = 0x92
    | Q06UDH = 0x72
    | Q12H = 0x24
    | Q12PH = 0x41
    | Q12PRH = 0x43
    | Q13UDEH = 0x93
    | Q13UDH = 0x73
    | Q25H = 0x25
    | Q25PH = 0x42
    | Q25PRH = 0x44
    | Q26UDEH = 0x94
    | Q26UDH = 0x74
    | Q2A = 0x11
    | Q2A_S1 = 0x12
    | Q3A = 0x13
    | Q4A = 0x14
    | QS001 = 0x60
    | A0J2H = 0x102
    | A171SH = 0x601
    | A172SH = 0x602
    | A173UH = 0x604
    | A1FX = 0x103
    | A1N = 0x106
    | A1S = 0x104
    | A1SH = 0x105
    | A273UH = 0x603
    | A2A = 0x10C
    | A2C = 0x107
    | A2N = 0x108
    | A2SH = 0x109
    | A2U = 0x10E
    | A2USH = 0x10F
    | A3A = 0x10D
    | A3N = 0x10A
    | A3U = 0x110
    | A4U = 0x111
    | FX0 = 0x201
    | FX0N = 0x202
    | FX1 = 0x203
    | FX1N = 0x207
    | FX1S = 0x206
    | FX2 = 0x204
    | FX2N = 0x205
    | FX3U = 0x208
    | CPU = 0x401


/// Melsec R-series Cpu types
type RCpuType =
    | Q03UDV = 0xD1                   // Q03UDV
    | Q04UDV = 0xD2                   // Q04UDV
    | Q06UDV = 0xD3                   // Q06UDV
    | Q13UDV = 0xD4                   // Q13UDV
    | Q26UDV = 0xD5                   // Q26UDV
    | L02S = 0xA3                     // L02S
    | L26 = 0xA4                      // L26
    | L06 = 0xA5                      // L06
    | Q24DHC_V = 0x59                 // Q24DHC_V
    | Q172 = 0x621                    // Q172/Q172H/Q172NC
    | Q173 = 0x622                    // Q173/Q173H/Q173NC
    | Q172D = 0x625                   // Q172D
    | Q173D = 0x626                   // Q173D
    | Q170M_P = 0x629                 // Q170M
    | Q172DS = 0x62A                  // Q172DS
    | Q173DS = 0x62B                  // Q173DS
    | R04 = 0x1001                    // R04
    | R08 = 0x1002                    // R08
    | R16 = 0x1003                    // R16
    | R32 = 0x1004                    // R32
    | R32SF = 0x1124                  // R32
    | R120 = 0x1005                   // R120
    | R16MT = 0x1011                  // R16MT
    | R32MT = 0x1012                  // R32MT
    | Q2A = 0x11                      // Q2A
    | Q2AS1 = 0x12                    // Q2AS1
    | Q3A = 0x13                      // Q3A
    | Q4A = 0x14                      // Q4A
    | Q00J = 0x30                     // Q00J
    | Q00 = 0x31                      // Q00
    | Q01 = 0x32                      // Q01
    | Q02 = 0x22                      // Q02(H) Q
    | Q06 = 0x23                      // Q06H   Q
    | Q12 = 0x24                      // Q12H   Q
    | Q25 = 0x25                      // Q25H   Q
    | Q12PH = 0x41                    // Q12PH
    | Q25PH = 0x42                    // Q25PH
    | Q12PRH = 0x43                   // Q12PRH
    | Q25PRH = 0x44                   // Q25PRH
    | Q25SS = 0x55                    // Q25SS
    | Q02U = 0x83                     // Q02U
    | Q03UD = 0x70                    // Q03UD
    | Q04UDH = 0x71                   // Q04UDH
    | Q06UDH = 0x72                   // Q06UDH
    | Q03UDE = 0x90                   // Q03UDE
    | Q04UDEH = 0x91                  // Q04UDEH
    | Q06UDEH = 0x92                  // Q06UDEH
    | Q13UDEH = 0x93                  // Q13UDEH
    | Q26UDEH = 0x94                  // Q26UDEH
    | Q02PH = 0x45                    // Q02PH  Q
    | Q06PH = 0x46                    // Q06PH  Q
    | Q13UDH = 0x73                   // Q13UDH Q
    | Q26UDH = 0x74                   // Q26UDH Q
    | Q00UJ = 0x80                    // Q00UJ
    | Q00U = 0x81                     // Q00U
    | Q01U = 0x82                     // Q01U
    | Q10UDH = 0x75                   // Q10UDH
    | Q20UDH = 0x76                   // Q20UDH
    | Q10UDEH = 0x95                  // Q10UDEH
    | Q20UDEH = 0x96                  // Q20UDEH
    | Q50UDEH = 0x98                  // Q50UDEH
    | Q100UDEH = 0x9A                 // Q100UDEH
    | A0J2H = 0x102                   // A0J2H
    | A1FX = 0x103                    // A1FX
    | A1S = 0x104                     // A1SA1SJ
    | A1SH = 0x105                    // A1SHA1SJH
    | A1N = 0x106                     // A1(N)
    | A2C = 0x107                     // A2CA2CJ
    | A2N = 0x108                     // A2(N)A2S
    | A2SH = 0x109                    // A2SH
    | A3N = 0x10A                     // A3(N)
    | A2A = 0x10C                     // A2A
    | A3A = 0x10D                     // A3A
    | A2U = 0x10E                     // A2UA2US
    | A2USHS1 = 0x10F                 // A2USHS1
    | A3U = 0x110                     // A3U
    | A4U = 0x111                     // A4U
    | Q02_A = 0x141                   // Q02(H)
    | Q06_A = 0x142                   // Q06H
    | L02 = 0xA1                      // L02
    | L26BT = 0xA2                    // L26-BT
    | Q12DC_V = 0x58                  // Q12DC_V
    | QS001 = 0x60                    // QS001
    | FX0 = 0x201                     // FX0/FX0S
    | FX0N = 0x202                    // FX0N
    | FX1 = 0x203                     // FX1
    | FX2 = 0x204                     // FX2/FX2C
    | FX2N = 0x205                    // FX2N/FX2NC
    | FX1S = 0x206                    // FX1S
    | FX1N = 0x207                    // FX1N
    | FX3UC = 0x208                   // FX3UC
    | FX3G = 0x209                    // FX3G
    | CPU_BOARD = 0x401               // NETWORK BOARD
    | A171SH = 0x601                  // A171SH
    | A172SH = 0x602                  // A172SH
    | A273UH = 0x603                  // A273UH
    | A173UH = 0x604                   // A173UH



/// Melsec CPU discriminated union types
type CpuType =
    | QnACpu of QnACpuType
    | RCpu of RCpuType
    member x.GetInteger() =
        match x with
        | QnACpu(t) -> (int)t
        | RCpu(t) -> (int)t


/// Melsec Act Interface types
type ActType =
    | QJ71E71TCP
    | QJ71E71UDP
    | AJ71E71TCP
    | AJ71E71UDP
    | QNUDECPUTCP
    | QNUDECPUUDP


/// Melsec Network Interface types
type NetworkCardInterface = 
    | QJ71E71TCP of ACTETHERLib.IActQJ71E71TCP3
    | QJ71E71UDP of ACTETHERLib.IActQJ71E71UDP3
    | AJ71E71TCP of ACTETHERLib.IActAJ71E71TCP3
    | AJ71E71UDP of ACTETHERLib.IActAJ71E71UDP3
    | QNUDECPUTCP of ACTETHERLib.IActQNUDECPUTCP3
    | QNUDECPUUDP of ACTETHERLib.IActQNUDECPUUDP3

    
//    override x.ToString() =
//        match x with
//        | QJ71E71TCP(_) -> "QJ71E71TCP3"
//        | QJ71E71UDP(_)  -> "QJ71E71UDP3"
//        | AJ71E71TCP(_)  -> "AJ71E71TCP3"
//        | AJ71E71UDP(_)  -> "AJ71E71UDP3"
//        | QNUDECPUTCP(_) -> "QNUDECPUTCP3"
//        | QNUDECPUUDP(_) -> "QNUDECPUUDP3"
//
