// the definition of the pipe-forward operator
let (|>) x f = f x


// http://tomasp.net/blog/async-sequences.aspx/
//
// Asynchronous computation that produces either end of a sequence
// (Nil) or the next value together with the rest of the sequence.
type AsyncSeq<'T> = Async<AsyncSeqInner<'T>> 
and AsyncSeqInner<'T> =
  | Nil
  | Cons of 'T * AsyncSeq<'T>


type Option<'T> =
    | Some of 'T
    | None