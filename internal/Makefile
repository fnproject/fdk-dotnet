RAW_VERSION=$(shell grep 'private static readonly string version' ./src/Fnproject.Fn.Fdk/Version.cs | cut -d '"' -f 2)
FDK_VERSION=$(RAW_VERSION)$(if $(BLD_BRANCH_SUFFIX),.$(BLD_NUMBER)$(BLD_BRANCH_SUFFIX),)
ifeq ($(DOTNET_VERSION), 3.1)
FRAMEWORK_VERSION=netcoreapp3.1
else ifeq ($(DOTNET_VERSION), 6.0)
FRAMEWORK_VERSION=net6.0
else ifeq ($(DOTNET_VERSION), 8.0)
FRAMEWORK_VERSION=net8.0
endif

.PHONY: set_fdk_version set_image_version build unit_test setup_test_image_build

set_fdk_version:
	echo "version: \"$(FDK_VERSION)\"" > $(BLD_INPUT_DIR)/buildOverrideVariables.conf

set_image_version:
	echo "version: \"$(DOTNET_VERSION)-$(FDK_VERSION)$(if $(VERSION_SUFFIX),-$(VERSION_SUFFIX),)\"" > $(BLD_INPUT_DIR)/buildOverrideVariables.conf

build:
	yum install -y dotnet-sdk-8.0 dotnet-sdk-6.0 dotnet-sdk-3.1
	dotnet build -c Release  -p:PackageVersion=$(VERSION)
	dotnet pack -o $(BLD_INPUT_DIR)/fdk-nuget-pkg -c Release -p:packageVersion=$(VERSION)

unit_test:
	yum install -y aspnetcore-runtime-8.0 aspnetcore-runtime-6.0 aspnetcore-runtime-3.1 dotnet-sdk-8.0 dotnet-sdk-6.0 dotnet-sdk-3.1
	dotnet --version
	dotnet --list-sdks
	dotnet test -l "console;verbosity=normal"

setup_test_image_build:
    # Build tag for the test image
	echo "version: \"dotnet-$(DOTNET_VERSION)-$(FDK_VERSION)\"" >$(BLD_INPUT_DIR)/buildOverrideVariables.conf
	echo "fdkVersion: \"$(FDK_VERSION)\"" >>$(BLD_INPUT_DIR)/buildOverrideVariables.conf
	echo "buildImageVersion: \"$(DOTNET_VERSION)-$(FDK_VERSION)-dev\"" >>$(BLD_INPUT_DIR)/buildOverrideVariables.conf
	echo "runtimeImageVersion: \"$(DOTNET_VERSION)-$(FDK_VERSION)\"" >>$(BLD_INPUT_DIR)/buildOverrideVariables.conf
	cat $(BLD_INPUT_DIR)/buildOverrideVariables.conf
	find $(BLD_INPUT_DIR)/internal/test-images -name '*.csproj' -exec sed -i".bak" "s/LATEST/$(FDK_VERSION)/g" "{}" \;
	find $(BLD_INPUT_DIR)/internal/test-images -name '*.csproj' -exec sed -i".bak" "s/TARGET/$(FRAMEWORK_VERSION)/g" "{}" \;
	find $(BLD_INPUT_DIR)/internal/test-images -name '*.csproj' -exec sh -c 'cp "fdk-nuget-pkg/Fnproject.Fn.Fdk.$(FDK_VERSION).nupkg" "$$(dirname {})"' \;