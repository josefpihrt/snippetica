## Snippetica.CSharp

### Snippet Browser
* Browse all available snippets with [Snippet Browser](http://pihrt.net/snippetica/snippets?engine=vscode&language=csharp).

### Quick Reference

* Default access modifier is **public**.

#### Member Declaration

Shortcut | Description | Comment
-------- | ----------- | -------
\_|interface member declaration|prefix
c|class declaration|\-
cr|constructor declaration|\-
de|delegate declaration|\-
em|enum declaration|\-
et|event declaration|\-
f|field declaration|\-
ie|inteface declaration|\-
ir|indexer declaration|\-
k|constant declaration|\-
m|method declaration|\-
p|property declaration|\-
pp|property declaration \(expanded\)|\-
st|struct declaration|\-

#### Modifer

Shortcut | Description | Comment
-------- | ----------- | -------
i|internal \(Friend\)|prefix
p|private|prefix
r|read\-only|prefix \(after access modifier\)
s|static \(Shared\)|prefix \(after access modifier\)
v|virtual \(Overridable\)|prefix \(after access modifier\)

#### Statement

Shortcut | Description | Comment
-------- | ----------- | -------
fe|foreach statement|\-
fr|for statement|\-
if|if statement|\-
re|return statement|\-
sh|switch statement|\-
t|try statement|\-
tw|throw statement|\-
u|using statement|\-
we|while statement|\-

#### Operator

Shortcut | Description | Comment
-------- | ----------- | -------
co|conditional operator|\-
n|new object creation|\-
no|nameof operator|\-
oo|operator overload|\-
t|explict cast operator|\-
to|typeof operator|\-
x|logical not operator|prefix

#### Type

Shortcut | Description | Comment
-------- | ----------- | -------
a|Array|\-
b|Boolean|\-
dt|DateTime|\-
i|Int32|\-
l|List&lt;T&gt;|\-
o|Object|\-
rr|Reader|suffix
s|String|\-
wr|Writer|suffix

#### Other

Shortcut | Description | Comment
-------- | ----------- | -------
\_|with initializer|suffix
\_|with parameters|suffix
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
y|yield|prefix

### List of Selected Snippets

Shortcut | Title
-------- | -----
\_ir|[interface indexer](InterfaceIndexer.snippet)
\_m|[interface method](InterfaceMethod.snippet)
\_p|[interface property](InterfaceProperty.snippet)
\_rp|[interface read\-only property](InterfaceReadOnlyProperty.snippet)
b|[braces](Braces.snippet)
c|[public class](PublicClass.snippet)
catch|[catch clause](Catch.snippet)
co|[conditional operator](ConditionalOperator.snippet)
con|[conditional operator \(equal to null\)](ConditionalOperatorEqualToNull.snippet)
coxn|[conditional operator \(not equal to null\)](ConditionalOperatorNotEqualToNull.snippet)
cr|[public constructor](PublicConstructor.snippet)
d|[default keyword](DefaultKeyword.snippet)
da|[Debug\.Assert](DebugAssert.snippet)
de|[public delegate](PublicDelegate.snippet)
dispose|[dispose pattern](Dispose.snippet)
don|[do while null](DoWhileNull.snippet)
doxn|[do while not null](DoWhileNotNull.snippet)
dt|[DateTime type](DateTimeType.snippet)
dw|[Debug\.WriteLine](DebugWriteLine.snippet)
e|[else clause](Else.snippet)
eif|[else\-if](ElseIf.snippet)
em|[public enum](PublicEnum.snippet)
equals|[Equals and GetHashCode](EqualsAndGetHashCode.snippet)
f|[public field](PublicField.snippet)
fe|[foreach statement](ForEach.snippet)
finally|[finally clause](Finally.snippet)
fr|[for statement](For.snippet)
frr|[for statement \(reversed\)](ForReversed.snippet)
g|[ type parameter](TypeParameter.snippet)
ge|[IEnumerable&lt;T&gt; type](IEnumerableOfTType.snippet)
ie|[public interface](PublicInterface.snippet)
ifn|[if equal to null](IfEqualToNull.snippet)
iftp|[if TryParse](IfTryParse.snippet)
ifx|[if not](IfNot.snippet)
ifxn|[if not equal to null](IfNotEqualToNull.snippet)
ifxtp|[if not TryParse](IfNotTryParse.snippet)
ir|[public indexer](PublicIndexer.snippet)
k|[public const](PublicConst.snippet)
l|[lambda expression](LambdaExpression.snippet)
l2|[lambda expression with 2 parameters](LambdaExpressionWithTwoParameters.snippet)
m|[public method](PublicMethod.snippet)
n|[equal to null](EqualToNull.snippet)
no|[nameof operator](NameOfOperator.snippet)
o|[object keyword](ObjectKeyword.snippet)
p|[public auto property](PublicAutoProperty.snippet)
pa|[parameter array ](ParameterArray.snippet)
ppif|[\#if directive](PreprocessorDirectiveIf.snippet)
ppife|[\#if\-\#else directive](PreprocessorDirectiveIfElse.snippet)
ppr|[\#region directive](PreprocessorDirectiveRegion.snippet)
pps|[public auto property with private setter](PublicAutoPropertyWithPrivateSet.snippet)
r|[return keyword](ReturnKeyword.snippet)
ref|[return false](ReturnFalse.snippet)
ren|[return null](ReturnNull.snippet)
ret|[return true](ReturnTrue.snippet)
rf|[public read\-only field](PublicReadOnlyField.snippet)
rp|[public read\-only auto property](PublicReadOnlyAutoProperty.snippet)
rpp|[public read\-only property](PublicReadOnlyProperty.snippet)
s|[string keyword](StringKeyword.snippet)
sc|[public static class](PublicStaticClass.snippet)
scr|[static constructor](StaticConstructor.snippet)
sf|[public static field](PublicStaticField.snippet)
sh|[switch statement](Switch.snippet)
sm|[public static method](PublicStaticMethod.snippet)
sne|[string\.IsNullOrEmpty](StringIsNullOrEmpty.snippet)
snw|[string\.IsNullOrWhiteSpace](StringIsNullOrWhiteSpace.snippet)
srf|[public static read\-only field](PublicStaticReadOnlyField.snippet)
srp|[public static read\-only auto property](PublicStaticReadOnlyAutoProperty.snippet)
srpp|[public static read\-only property](PublicStaticReadOnlyProperty.snippet)
st|[public struct](PublicStruct.snippet)
t|[explicit cast operator](ExplicitCastOperator.snippet)
tc|[try\-catch](TryCatch.snippet)
tcf|[try\-catch\-finally](TryCatchFinally.snippet)
td|[TODO comment](TodoComment.snippet)
tf|[try\-finally](TryFinally.snippet)
to|[typeof operator](TypeOfOperator.snippet)
twn|[throw new](ThrowNew.snippet)
u|[using statement](Using.snippet)
us|[using static directive](UsingStatic.snippet)
v|[local variable declaration](Variable.snippet)
va|[array variable](ArrayOfTVariable.snippet)
vb|[Boolean variable](BooleanVariable.snippet)
vi|[Int32 variable](Int32Variable.snippet)
vm|[public virtual method](PublicVirtualMethod.snippet)
vn|[new variable](NewVariable.snippet)
vp|[public virtual auto property](PublicVirtualAutoProperty.snippet)
vrp|[public virtual read\-only auto property](PublicVirtualReadOnlyAutoProperty.snippet)
vrpp|[public virtual read\-only property](PublicVirtualReadOnlyProperty.snippet)
vs|[String variable](StringVariable.snippet)
vt|[local variable declaration with explicit cast operator](VariableWithExplicitCast.snippet)
we|[while statement](While.snippet)
wen|[while null](WhileNull.snippet)
wexn|[while not null](WhileNotNull.snippet)
where|[generic type constraint](GenericTypeConstraint.snippet)
xn|[not equal to null](NotEqualToNull.snippet)
xsne|[\!string\.IsNullOrEmpty](NotStringIsNullOrEmpty.snippet)
xsnw|[\!string\.IsNullOrWhiteSpace](NotStringIsNullOrWhiteSpace.snippet)
yb|[yield break statement](YieldBreak.snippet)
yr|[yield return statement](YieldReturn.snippet)
