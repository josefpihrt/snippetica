# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [Unreleased]

### Changed

- Rename default branch to `main`.
- Format changelog according to 'Keep a Changelog' ([#17](https://github.com/josefpihrt/roslynator/pull/17)).

-----
<!-- Content below does not adhere to 'Keep a Changelog' format -->

## 0.9.2 (2021-11-14)

### New Snippets for C#

* Lambda expression without parameters (l0)
* DateTimeOffset (dto)
* KeyValuePair (kvp)
* Record (rd)
* Record struct (rdst)
* Readonly record struct (rrdst)
* Attribute a
  * AttributeUsage (au)
  * Conditional (c)
  * DebuggerDisplay (dd)
  * DebuggerStepThrough (dst)
  * DefaultValue (dv)
  * Description (d)
  * Flags (f)
  * Obsolete (o)
  * Required (r)
  * TypeConverter (tc)

## 0.9.1 (2019-04-14)

* Put back snippets that were accidentally removed in 0.9.0 (ifxtp, ifx, ifxn, xn, xsne, xsnw).

### Snippets for C# and VB

* Change shortcut ge to j
* Change shortcut gc to j
* Change shortcut vc to vn
* Change shortcut vcx to vnx

## 0.9.0 (2019-03-21)

### Snippets for C# and VB

* Change shortcut ge to j
* Change shortcut gc to j
* Change shortcut vc to vn
* Change shortcut vcx to vnx

## 0.8.0 (2018-04-04)

### New Snippets for C# and VB

* Read-only indexer (rir)
* ThrowNewObjectDisposedException (twn ode)

### New Snippets for C#

* Read-only struct (rst)

## 0.7.0 (2017-12-13)

* Rewrite C++ snippets
* Change shortcut for '...WithInitializer' snippets from '_' to 'x' (C# and VB)
* Remove '...WithArguments' snippets and '...WithParameters' snippets (C# and VB)

## 0.6.0 (2017-09-24)

### New Shortcuts

Shortcut | Description | Comment
-------- | ----------- | -------
x|logical not operator|prefix

### New Snippets

Language | Shortcut | Title
-------- | -------- | -----
C\+\+|eif|else\-if
C\#|coxn|conditional operator \(not equal to null\)
C\#|con|conditional operator \(equal to null\)
C\#|doxn|do\-while not null
C\#|don|do\-while null
C\#|l2|lambda expression with 2 parameters
C\#|wexn|while not null
C\#|wen|while null
VB|l2|lambda expression with 2 parameters
VB|wexn|while not null
VB|wen|while null

## 0.5.2 (2017-05-28)

### New Snippets

Language | Shortcut | Title
-------- | -------- | -----
C\#|iftp|if TryParse
C\#|ifftp|if not TryParse
C\#|u|StreamReader variable
C\#|u|StreamWriter variable
C\#|u|StringReader variable
C\#|u|StringWriter variable
C\#|u|XmlReader variable
C\#|u|XmlWriter variable
C\#|us|using static directive
VB|iftp|if TryParse
VB|ifftp|if not TryParse
VB|u|StreamReader variable
VB|u|StreamWriter variable
VB|u|StringReader variable
VB|u|StringWriter variable
VB|u|XmlReader variable
VB|u|XmlWriter variable

## 0.5.1 (2017-03-09)

### New Snippets

* 'virtual/Overridable' snippets

## 0.5.0 (2016-12-30)

### New Snippets

Language | Shortcut | Title
-------- | -------- | -----
C\#|u|IEnumerator\<T\> variable
C\#|u|using variable
C\#|vb|Boolean variable
C\#|vi|Int32 variable
C\#|vs|String variable
C\#|vt|variable declaration with explicit cast operator
VB|u|IEnumerator\(Of T\) variable
VB|vb|Boolean variable
VB|vi|Int32 variable
VB|vs|String variable
VB|vt|variable with DirectCast

## 0.1.0 (2016-10-16)

* Initial release
