# Changelog
All notable changes to this project will be documented in this file, starting with version 1.1.0.x

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
## [1.2.3.x] - 2019-11-21
### Changed by [@tomidix](https://github.com/tomidix) & [@dominikjancik](https://github.com/dominikjancik) 
- Implemented [#25](https://github.com/tomidix/CSharpTradFriLibrary/issues/25) - Wrapper for setting CIE Yxy colors
- Credits for testing and fixes to [dominikjancik](https://github.com/dominikjancik)
- Fixed [#36](https://github.com/tomidix/CSharpTradFriLibrary/issues/36) - GenerateAppSecret Timeout lowered (to 10s) in TradfriUI project and now has an example on how to handle timeout

## [1.2.2.x] - 2019-11-21
### Changed by [@tomidix](https://github.com/tomidix)
- Fixed [#30](https://github.com/tomidix/CSharpTradFriLibrary/issues/30) - Custom mood activation not working properly when using mood with different settings for multiple bulbs in group
- Added TransitionTime property to TradfriMoodProperties 

## [1.2.1.x] - 2019-10-16
### Changed by [@tomidix](https://github.com/tomidix)
- Nuget packages updated because of [PeterO.Cbor vulnerability](https://github.com/peteroupc/CBOR/security/advisories/GHSA-cxw4-9qv9-vx5h) which was fixed in v4.0

## [1.2.0.x] - 2019-07-01
### Changed by [@johanjonsson1](https://github.com/johanjonsson1)
- Added basic control outlet support

## [1.1.0.x] - 2019-03-23
### Changed by [@tomidix](https://github.com/tomidix)
- Added UI example project
- Rewrite TradfriDevice Controller ObserveDevice method
- Fixed Issue [CoapClient instance needed for ObserveDevice method on DeviceController #21](https://github.com/tomidix/CSharpTradFriLibrary/issues/21)
- Fixed Issue [Lights are not turning on/off when observing device #22](https://github.com/tomidix/CSharpTradFriLibrary/issues/22)
- Minor tweaks and additions
