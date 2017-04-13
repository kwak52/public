module Dsu.Driver.UpsHttp

open System
open Http



/// <summary>
/// APC Smart 1000 UPS 에 대한 HTTP protocol 을 이용한 manager.
/// TODO : HTTP protocol 을 이용한 방식보다는 고유의 driver 를 이용한 방식이 존재하는지 확인해야 한다.
/// </summary>
[<Obsolete("Do not use. Use UpsManager instead.")>]
type HttpUpsManager(ip:string, port:int) =
    let login() =
        let loginUrl = sprintf "http://%s:%d/Forms/login1" ip port 
        let postData = sprintf "login_username=%s&login_password=%s&submit=Log+On" Ups.upsUserId Ups.upsPassword
        loginWebpage loginUrl postData

    member val Ip = ip with get
    member val Port = port with get

    /// <summary>
    /// 온도 센서를 통한 온도값 반환.
    /// 해당 url (uiotemp.htm) 은 계정을 통한 로그인이 먼저 수행되어야 하므로, 
    /// 기본 login page 에 login 을 수행해서 cookie 와 redirection 값을 획득한다.
    /// 획득된 정보를 기반으로 uiotemp.htm page 를 가져와서 온도 표시된 정보를 추출한다.
    /// TODO : text 기반 parsing 을 Html parsing 기반으로 필요한다면 변경해야 한다.  가령 추출한 정보가 복수개라던가...
    /// </summary>
    member __.GetTemperature() = 
        let response = login()
        let (cookies, redirect) = (response.Cookies, response.Headers.["Location"])
        let temperatureUrl = redirect + "uiotemp.htm"

        let response = getUrlWithCookies temperatureUrl cookies
        let targetLine = 
            let page = extractHtmlFromResponse response
            let lines = page.Split([|'\r'; '\n'|])
            lines
                |> Array.filter(fun s -> s.Contains("&deg;&nbsp;C</td>"))
                |> Array.head
        targetLine


//        let hd = HtmlDocument.Parse(page)
//        hd.Elements
//        let xn s = XName.Get(s)
//        let td =
//            hd.Descendants(xn "tr")
//                |> Seq.filter(fun (p: XElement) -> p.Attribute(xn "class").Value = "shade")
//                |> Seq.head
//
//        hd.ToString()


    new (ip:string) = HttpUpsManager(ip, 80)
    new () = HttpUpsManager(Ups.upsIp, 80)