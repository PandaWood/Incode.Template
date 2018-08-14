NetCode.Template produces text output from executable .NET code templates intermixed with text, similar to old-style ASP code.

This is a continuation of Lasse V. Karlsen's texttemplate project on CodePlex (lasse@vkarlsen.no)
Original URL http://texttemplate.codeplex.com/

I've got it compiling and passing all tests (on mono too)...
So it's probably good to go from here, but work to be done

## NetCode-Template
This class library for .NET 3.5 and 4.0 allows you to write code templates, 
and execute them at runtime, producing textual output. 
You can use these to produce code files, mail merge content, 
or just about any other output.


### Features
Language: C# 3.0
Runtime: .NET 3.5 Client Profile or .NET 4.0 Client Profile

(Note for NetCodeT - this will of course be updated)

Language in template is C# 3.0 or 4.0
Can reference other assemblies, like ones from your program.

Generic parameterization, you can pass custom data structures to the template engine, 
to control the template execution and provide information to it
All source code available, fully documented (that is, all public types and methods will have full XML documentation, source itself hopefully won't need comments)

####-------------------
Version 2.0 (NB I will make the first release of this new project v3.0 then)

This release contains major new features, as well as all bugfixes that went into 1.1.

#### New features
Generic Template<T> class, for passing type-safe parameters to template execution
Ability to "unwrap" parameters, exposing the public properties as parameters instead of the wrapper parameter
{<%+} code block to add code outside of the generator method, in order to be able to add extra methods and nested classes

##### Breaking changes
Async methods have been removed

#### Issues fixed for this release
Invalid/unknown directives did not throw an exception during template parsing. (also in 1.1)
When {<%+} blocks have been started, the programmer cannot switch back to normal blocks, {<%} and {<%=} without having them ordered before the {<%+} blocks.
A percentage-sign on its own outside a code block breaks the template parser. (also in 1.1)
A less-than sign on its own outside a code block breaks the template parser. (also in 1.1)

#### Changes
ReSharper settings changed to avoid too much code reordering and reformatting to avoid merge conflicts during parallel development. (also in 1.1)
Introduced TemplateSyntaxException to replace InvalidOperationException for some problems related to template parsing. (also in 1.1)

