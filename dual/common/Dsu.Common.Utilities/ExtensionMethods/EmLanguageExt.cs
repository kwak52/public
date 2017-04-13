using System;
using LanguageExt;
using LanguageExt.SomeHelp;

namespace Dsu.Common.Utilities.ExtensionMethods
{
	public static class EmLanguageExt
	{
		public static Option<T> ToOption<T>(this T value) => value.ToSome();
		public static Option<T> ReturnOption<T>(this T value) => value.ToSome();

		public static Option<R> MapOption<T, R>(this Func<T, R> mapper, Option<T> self) => self.Map(mapper);

		/// <summary>
		/// Option value opt 가 IsSome 상태일 때에만 정상적인 값을 반환.  IsNone 상태일 때는 exception 발생
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="opt"></param>
		/// <returns></returns>
		public static T GetValueUnsafe<T>(this Option<T> opt)
		{
			if ( opt.IsNone )
				throw new ValueIsNoneException();

			return opt.IfNone(default(T));
		}





		public static Try<T> ToTry<T>(this T value) => () => value;
		public static Try<T> ToTry<T>(this Func<T> tryDel) => LanguageExt.Prelude.Try<T>(tryDel);
		public static Try<T> ReturnTry<T>(this T value) => value.ToTry();
		public static Try<T> ReturnTry<T>(this Func<T> tryDel) => LanguageExt.Prelude.Try<T>(tryDel);
		public static Try<R> MapTry<T, R>(this Func<T, R> mapper, Try<T> self) => self.Map(mapper);
	}
}
