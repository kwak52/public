
Driver Architecture

	- XXX = {Paix, NiDaq, ...}
	1. 호출 방식
		XXX.createManager(..)
		XXX.manager().DoSomething(...)
	1. 내부 구현
		- createManager() 에서 Singleton 객체에 생성된 manager 객체를 할당
		- manager singleton 을 통한 operation 에 exception 발생시, singleton 객체에 새로운 객체를 생성해서 할당
		- XXX.manager() 를 통한 접근은 exception 발생과 무관하게 새로운 running 객체를 접근할 수 있음.

		let mutable private xxxManagerSingleton: XXXManager option = None
		let rec createManager parameters =
			try
				let xxx = new XXXManager(parameters)
				xxx.ProcException <- fun exn -> createManager ip true |> ignore
				xxxManagerSingleton <- Some(paix)
			with exn ->
				DriverExceptionSubject.OnNext(XXXException(exn))

		/// XXX manager 객체에 대한 interface 를 생성한다.
		let createManagerSimple() = createManager parameters
