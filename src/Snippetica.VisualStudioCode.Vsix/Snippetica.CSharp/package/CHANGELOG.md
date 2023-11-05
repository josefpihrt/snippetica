# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [Unreleased]

## [2.0.0] - 2023-11-05

### Added

- Add snippet for 'while not' (`wx`) ([PR](https://github.com/josefpihrt/snippetica/pull/54)).
- Add 'abstract' snippets (`a`) ([PR](https://github.com/josefpihrt/snippetica/pull/66)).
- Add 'protected' snippets (`d`) ([PR](https://github.com/josefpihrt/snippetica/pull/68), [PR](https://github.com/josefpihrt/snippetica/pull/71)).
- Add snippets for VB keywords ([PR](https://github.com/josefpihrt/snippetica/pull/74)).
  - `Object` keyword (`o`)
  - `Return` keyword (`r`)
  - `String` keyword (`s`)

### Changed

- Migrate documentation to [Docusaurus](https://josefpihrt.github.io/docs/snippetica) ([PR](https://github.com/josefpihrt/snippetica/pull/56)).
- Update logo ([PR](https://github.com/josefpihrt/snippetica/pull/60), [PR](https://github.com/josefpihrt/snippetica/pull/62)).
- Bump Roslynator analyzers to `4.6.1` ([PR](https://github.com/josefpihrt/snippetica/pull/67)).
- Remove parentheses from `nameof` snippet ([PR](https://github.com/josefpihrt/snippetica/pull/72)).

### Removed

- Remove option to choose member type (method, property etc.) [PR](https://github.com/josefpihrt/snippetica/pull/63)).
- Remove snippet for read-only property (`rpp`) [PR](https://github.com/josefpihrt/snippetica/pull/69)).
- Remove all but C#, VB and C++ snippets [PR](https://github.com/josefpihrt/snippetica/pull/70)).

## [1.0.0] - 2022-10-17

### Added

- Add CODEOWNERS file ([#21](https://github.com/josefpihrt/snippetica/pull/21)).
- Add support for Visual Studio 2022 ([#22](https://github.com/josefpihrt/snippetica/pull/22)).
- Add snippet for `KeyValuePair.Create` (`kvpc`) ([#24](https://github.com/josefpihrt/snippetica/pull/24)), ([#26](https://github.com/josefpihrt/snippetica/pull/26)).
- Add snippet for 'using declaration' (`uv`) ([#27](https://github.com/josefpihrt/snippetica/pull/27)).
- Add snippet for 'switch expression' (`swe`) ([#34](https://github.com/josefpihrt/roslynator/pull/34)).

### Changed

- Rename default branch to `main`.
- Format changelog according to 'Keep a Changelog' ([#17](https://github.com/josefpihrt/snippetica/pull/17)).
- Update projects to .NET 4.8 ([#18](https://github.com/josefpihrt/snippetica/pull/18)).
- Move solution file to `src` ([#19](https://github.com/josefpihrt/snippetica/pull/19)).
- Replace ruleset file with EditorConfig ([#20](https://github.com/josefpihrt/snippetica/pull/20)).
- Migrate projects to a new csproj format ([#23](https://github.com/josefpihrt/snippetica/pull/23)).
- Add `Directory.Build.props` ([#23](https://github.com/josefpihrt/snippetica/pull/23)).
- Simplify snippet 'using statement' (`u`) ([#27](https://github.com/josefpihrt/snippetica/pull/27)).
- Use pattern matching to check for null ([#29](https://github.com/josefpihrt/snippetica/pull/29)).
- Change shortcut for 'interface declaration' from `ie` to `i` ([#30](https://github.com/josefpihrt/snippetica/pull/30)), ([#48](https://github.com/josefpihrt/snippetica/pull/48)).
- Change shortcut for 'while' from `we` to `w` ([#33](https://github.com/josefpihrt/snippetica/pull/33)).
- Change shortcut for 'switch statement' from `sh` to `sw` ([#35](https://github.com/josefpihrt/roslynator/pull/35)), ([#38](https://github.com/josefpihrt/roslynator/pull/38)).
- Change shortcut for 'enum' from `em` to `en` ([#31](https://github.com/josefpihrt/roslynator/pull/31)).
- Change shortcut for 'record declaration' from `rd` to `re` ([#39](https://github.com/josefpihrt/snippetica/pull/39)).
- Change shortcut for 'indexer' from `ir` to `in` ([#40](https://github.com/josefpihrt/snippetica/pull/40)).
- Change shortcut for 'event declaration' from `et` to `ev` ([#42](https://github.com/josefpihrt/roslynator/pull/42)).
- Change shortcut for 'throw statement' from `tw` to `th` ([#41](https://github.com/josefpihrt/roslynator/pull/41)).
- Change shortcut for constructor from `cr` to `co` ([#43](https://github.com/josefpihrt/snippetica/pull/43)).
- Change shortcut for ternary conditional operator from `co` to `cop` ([#43](https://github.com/josefpihrt/snippetica/pull/43)).
- Change shortcut for 'module declaration' from `me` to `mo` ([#44](https://github.com/josefpihrt/snippetica/pull/44)).
- Change shortcut for 'destructor declaration' from `dr` to `de` ([#45](https://github.com/josefpihrt/snippetica/pull/45)).
- Change shortcut for 'namespace declaration' from `ns` to `na` ([#47](https://github.com/josefpihrt/snippetica/pull/47)).
- [C++] Change shortcut for 'cast' from `ct` to `ca` ([#50](https://github.com/josefpihrt/snippetica/pull/50)).
- [C++] Change shortcut for 'inline' from `il` to `i` ([#51](https://github.com/josefpihrt/snippetica/pull/51)).
- [C++] Change shortcut for 'do-while' from `dw` to `do` ([#52](https://github.com/josefpihrt/snippetica/pull/52)).

### Removed

- Remove snippet for type parameter (`g`) ([#28](https://github.com/josefpihrt/snippetica/pull/28)).
- Remove snippets where return type is either Int64 or DateTime ([#36](https://github.com/josefpihrt/snippetica/pull/36)).

## [0.9.2] 2021-11-14

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

## [0.9.1] 2019-04-14

* Put back snippets that were accidentally removed in 0.9.0 (ifxtp, ifx, ifxn, xn, xsne, xsnw).

## [0.9.0] 2019-03-21

* Change shortcut ge to j
* Change shortcut gc to j
* Change shortcut vc to vn
* Change shortcut vcx to vnx

## [0.7.0] 2017-12-13

* Change shortcut for '...WithInitializer' snippets from '_' to 'x'
* Remove '...WithArguments' snippets and '...WithParameters' snippets
