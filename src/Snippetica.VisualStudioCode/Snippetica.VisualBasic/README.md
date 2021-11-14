## Snippetica\.VisualBasic

### Snippet Browser

* Browse all available snippets with [Snippet Browser](http://pihrt.net/snippetica/snippets?engine=vscode&language=vb)\.

### Quick Reference

* Default access modifier is **Public**.

#### Member Declaration

Shortcut|Description|Comment
--------|-----------|-------
\_|interface member declaration|prefix
c|class declaration|\-
cr|constructor declaration|\-
de|delegate declaration|\-
em|enum declaration|\-
et|event declaration|\-
f|field declaration|\-
ie|interface declaration|\-
ir|indexer declaration|\-
k|constant declaration|\-
m|method declaration|\-
me|Module declaration|\-
p|property declaration|\-
pp|property declaration \(expanded\)|\-
rd|record declaration|\-
st|struct declaration|\-

#### Modifer

Shortcut|Description|Comment
--------|-----------|-------
i|internal \(Friend\)|prefix
p|private|prefix
r|read\-only|prefix \(after access modifier\)
s|static \(Shared\)|prefix \(after access modifier\)
v|virtual \(Overridable\)|prefix \(after access modifier\)

#### Statement

Shortcut|Description|Comment
--------|-----------|-------
fe|foreach statement|\-
fr|for statement|\-
if|if statement|\-
re|return statement|\-
sc|Select Case statement|\-
t|try statement|\-
tw|throw statement|\-
u|using statement|\-
we|while statement|\-

#### Operator

Shortcut|Description|Comment
--------|-----------|-------
gt|GetType operator|\-
n|new object creation|\-
no|nameof operator|\-
oo|operator overload|\-
tc|TryCast operator|\-
x|logical not operator|prefix

#### Type

Shortcut|Description|Comment
--------|-----------|-------
a|Array|\-
b|Boolean|\-
dt|DateTime|\-
i|Int32|\-
l|List\<T>|\-
o|Object|\-
rr|Reader|suffix
s|String|\-
wr|Writer|suffix

#### Other

Shortcut|Description|Comment
--------|-----------|-------
c|catch clause|\-
d|default keyword|\-
e|else clause|\-
f|finally clause|\-
g|generic type|prefix
g|type parameter|\-
l|lambda expression|\-
n|\(equals to\) null|\-
pa|parameter array|\-
pp|preprocessor directive|prefix
ps|private set|\-
r|return keyword|\-
v|local variable declaration|\-
x|with initializer|suffix
y|yield|prefix

### List of Selected Snippets

Shortcut|Title
--------|-----
\_et|[interface event](InterfaceEvent.snippet)
\_ett|[interface event with EventHandler\<T>](InterfaceEventWithEventHandlerOfT.snippet)
\_ir|[interface indexer](InterfaceIndexer.snippet)
\_m|[interface method](InterfaceMethod.snippet)
\_p|[interface property](InterfaceProperty.snippet)
\_rp|[interface read-only property](InterfaceReadOnlyProperty.snippet)
c|[Public class](PublicClass.snippet)
catch|[Catch clause](Catch.snippet)
cr|[Public constructor](PublicConstructor.snippet)
da|[Debug.Assert](DebugAssert.snippet)
de|[Public delegate](PublicDelegate.snippet)
dispose|[Dispose](Dispose.snippet)
do|[Do statement](Do.snippet)
dt|[DateTime type](DateTimeType.snippet)
dw|[Debug.WriteLine](DebugWriteLine.snippet)
e|[Else clause](Else.snippet)
eif|[ElseIf](ElseIf.snippet)
em|[Public enum](PublicEnum.snippet)
equals|[Equals and GetHashCode](EqualsAndGetHashCode.snippet)
f|[Public field](PublicField.snippet)
fe|[For Each statement](ForEach.snippet)
fr|[For statement](For.snippet)
frr|[For statement (reversed)](ForReversed.snippet)
g|[ type parameter](TypeParameter.snippet)
gt|[GetType](GetTypeOperator.snippet)
ie|[Public interface](PublicInterface.snippet)
ifn|[If Is Nothing](IfIsNothing.snippet)
iftc|[If TryCast IsNot Nothing](IfTryCastIsNotNothing.snippet)
iftp|[if TryParse](IfTryParse.snippet)
ifx|[If Not](IfNot.snippet)
ifxn|[If IsNot Nothing](IfIsNotNothing.snippet)
ifxtc|[If TryCast Is Nothing](IfTryCastIsNothing.snippet)
ifxtp|[if not TryParse](IfNotTryParse.snippet)
ir|[Public indexer](PublicIndexer.snippet)
k|[Public constant](PublicConstant.snippet)
l|[Lambda expression](LambdaExpression.snippet)
l0|[Lambda expression without parameters](LambdaExpressionWithoutParameters.snippet)
l2|[Lambda expression with 2 parameters](LambdaExpressionWithTwoParameters.snippet)
m|[Public method](PublicMethod.snippet)
me|[Public module](PublicModule.snippet)
n|[Is Nothing](IsNothing.snippet)
no|[NameOf operator](NameOfOperator.snippet)
p|[Public auto property](PublicAutoProperty.snippet)
pa|[parameter array ](ParameterArray.snippet)
ppif|[#If directive](PreprocessorDirectiveIf.snippet)
ppife|[#If-#Else directive](PreprocessorDirectiveIfElse.snippet)
ppr|[#Region directive](PreprocessorDirectiveRegion.snippet)
ref|[Return False](ReturnFalse.snippet)
ren|[Return Nothing](ReturnNothing.snippet)
ret|[Return True](ReturnTrue.snippet)
rf|[Public read-only field](PublicReadOnlyField.snippet)
rir|[Public read-only indexer](PublicReadOnlyIndexer.snippet)
rp|[Public read-only auto property](PublicReadOnlyAutoProperty.snippet)
rpp|[Public read-only Property](PublicReadOnlyProperty.snippet)
sc|[Select Case statement](SelectCase.snippet)
scr|[static constructor](StaticConstructor.snippet)
sf|[Public Shared field](PublicStaticField.snippet)
sm|[Public Shared method](PublicStaticMethod.snippet)
sne|[String.IsNullOrEmpty](StringIsNullOrEmpty.snippet)
snw|[String.IsNullOrWhiteSpace](StringIsNullOrWhiteSpace.snippet)
srf|[Public Shared read-only field](PublicStaticReadOnlyField.snippet)
srp|[Public Shared read-only auto property](PublicStaticReadOnlyAutoProperty.snippet)
srpp|[Public Shared read-only Property](PublicStaticReadOnlyProperty.snippet)
st|[Public structure](PublicStruct.snippet)
t|[CType operator](CTypeOperator.snippet)
tc|[Try-Catch](TryCatch.snippet)
tcf|[Try-Catch-Finally](TryCatchFinally.snippet)
td|[TODO comment](TodoComment.snippet)
tf|[Try-Finally](TryFinally.snippet)
twn|[Throw New](ThrowNew.snippet)
u|[Using statement](Using.snippet)
u\_er|[IEnumerator(Of T) variable](IEnumeratorOfTVariable.snippet)
u\_smrr|[StreamReader Variable](StreamReaderVariable.snippet)
u\_smwr|[StreamWriter Variable](StreamWriterVariable.snippet)
u\_srr|[StringReader Variable](StringReaderVariable.snippet)
u\_swr|[StringWriter Variable](StringWriterVariable.snippet)
u\_xmlrr|[XmlReader Variable](XmlReaderVariable.snippet)
u\_xmlwr|[XmlWriter Variable](XmlWriterVariable.snippet)
v|[local variable declaration](Variable.snippet)
va|[array variable](ArrayOfTVariable.snippet)
vb|[Boolean variable](BooleanVariable.snippet)
vi|[Int32 variable](Int32Variable.snippet)
vm|[Public Overridable method](PublicVirtualMethod.snippet)
vn|[- new variable](NewVariable.snippet)
vp|[Public Overridable auto property](PublicVirtualAutoProperty.snippet)
vrp|[Public Overridable read-only auto property](PublicVirtualReadOnlyAutoProperty.snippet)
vrpp|[Public Overridable read-only Property](PublicVirtualReadOnlyProperty.snippet)
vs|[String variable](StringVariable.snippet)
vt|[local variable with DirectCast](VariableWithDirectCast.snippet)
we|[While statement](While.snippet)
wen|[While Is Nothing](WhileIsNothing.snippet)
wexn|[While IsNot Nothing](WhileIsNotNothing.snippet)
xn|[IsNot Nothing](IsNotNothing.snippet)
xsne|[String.IsNullOrEmpty = False](NotStringIsNullOrEmpty.snippet)
xsnw|[String.IsNullOrWhiteSpace = False](NotStringIsNullOrWhiteSpace.snippet)

*\(Generated with [DotMarkdown](http://github.com/JosefPihrt/DotMarkdown)\)*