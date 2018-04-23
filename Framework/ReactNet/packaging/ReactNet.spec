%define DEV_VERSION beta-010

Name:       ReactNet
Summary:    ReactNativeTizen C# Framework
Version:    0.0.1
Release:    0
Group:      Development/Libraries
License:    MIT
Source0:    %{name}-%{version}.tar.gz
Source1:    %{name}.manifest

AutoReqProv: no

BuildRequires: pkgconfig(libtzplatform-config)
BuildRequires: dotnet-build-tools
BuildRequires: dotnet-nupkgs

%define Assemblies ReactNativeTizen

%description
%{summary}

%dotnet_import_sub_packages

%package -n yoga
Summary:   yoga
Version:   1.6
Release:   0
Group:     System/Libraries
License:   BSD

%description -n yoga
yoga

%prep
%setup -q
cp %{SOURCE1} .

%build
make -C yoga %{?jobs:-j%jobs}
for ASM in %{Assemblies}; do
%dotnet_build $ASM
%dotnet_pack $ASM/$ASM.nuspec %{version}%{?DEV_VERSION:-%{DEV_VERSION}}
done

%install
for ASM in %{Assemblies}; do
%dotnet_install_assembly $ASM
%dotnet_install_nuget $ASM
done
install -m644 ReactNativeTizen/Ref/Facebook.Yoga.dll 				${RPM_BUILD_ROOT}/usr/share/dotnet.tizen/framework/Facebook.Yoga.dll
install -m644 ReactNativeTizen/Ref/System.Reactive.dll 				${RPM_BUILD_ROOT}/usr/share/dotnet.tizen/framework/System.Reactive.dll
mkdir -p ${RPM_BUILD_ROOT}%{_libdir}
install -m644 JSCore/libJSCore_4.0.so ${RPM_BUILD_ROOT}%{_libdir}/libJSCore.so
install -m644 yoga/libyoga.so ${RPM_BUILD_ROOT}%{_libdir}/libyoga.so


%files
%manifest %{name}.manifest
%attr(644,root,root) %{dotnet_assembly_files}
%attr(644,root,root) /usr/share/dotnet.tizen/framework/Facebook.Yoga.dll
%attr(644,root,root) /usr/share/dotnet.tizen/framework/System.Reactive.dll
%attr(644,root,root) %{_libdir}/libJSCore.so

%files -n yoga
%manifest %{name}.manifest
%attr(644,root,root) %{_libdir}/libyoga.so