all: Program.cs
	mcs Program.cs \
		HttpRequest.cs
	

clean:
	rm Program.exe 
