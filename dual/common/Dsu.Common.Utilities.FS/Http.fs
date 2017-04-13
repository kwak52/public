module Http


open System.Text
open System.IO
open System.Net
open System.Collections.Specialized

/// <summary>
/// 주어진 url 을 이용해서 HttpWebRequest 객체 생성 및 기본 값 초기화
/// </summary>
/// <param name="url"></param>
let private createWebRequest (url:string) =
    let request = WebRequest.Create(url) :?> HttpWebRequest 
    request.UserAgent <- "Kefico-CPT/1.0"
    request.AllowWriteStreamBuffering <- true;
    request.ProtocolVersion <- HttpVersion.Version11
    request.AllowAutoRedirect <- true;
    request.ContentType <- "application/x-www-form-urlencoded"
    request

let private createWebRequestWithCookies (url:string) (cookies:CookieCollection) =
    let request = createWebRequest(url)
    request.CookieContainer <- new CookieContainer()
    request.CookieContainer.Add(cookies)
    request


/// <summary>
/// URL 과 post data 가 주어졌을 때, 해당 URL 에 post 해서 HttpWebResponse 객체를 반환한다.
/// response 를 통해서 cookie 및 결과 html page, redirection 정보 등을 얻을 수 있다.
/// </summary>
/// <param name="url"></param>
/// <param name="postData"></param>
let loginWebpage (url:string) (postData:string) : HttpWebResponse =
    let request = 
        let cookies = new CookieCollection()
        createWebRequestWithCookies url cookies
    
    request.AllowAutoRedirect <- false;
    request.Method <- WebRequestMethods.Http.Post

    let byteArray = Encoding.ASCII.GetBytes(postData)
    request.ContentLength <- (int64)byteArray.Length;
    let newStream = request.GetRequestStream(); //open connection
    newStream.Write(byteArray, 0, byteArray.Length); // Send the data.
    newStream.Close();

    let response = request.GetResponse() :?> HttpWebResponse
    response

/// <summary>
/// response:HttpWebResponse 로부터 html page 의 문자열을 반환한다.
/// </summary>
/// <param name="response"></param>
let extractHtmlFromResponse (response:HttpWebResponse) =
    use streamReader = new StreamReader(response.GetResponseStream())
    streamReader.ReadToEnd()


/// <summary>
/// URL 을 주어진 cookie 를 이용해서 GET method 로 방문한 결과를 반환한다.
/// </summary>
/// <param name="url"></param>
/// <param name="cookies"></param>
let getUrlWithCookies (url:string) (cookies:CookieCollection) : HttpWebResponse =
    let request = createWebRequestWithCookies url cookies
    request.AllowAutoRedirect <- true
    request.GetResponse() :?> HttpWebResponse
