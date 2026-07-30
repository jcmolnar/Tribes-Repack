function onPluginsLoaded() {
    echoc(2, "--------------------------------------");
    echoc(2, "xLoader Plugins Loaded");
    echoc(2, "--------------------------------------");
    echoc(3, "Loaded Plugins: " ~ $xLoader::LoadedCount);
    echoc(1, "Failed Plugins: " ~ $xLoader::FailedCount);
    echoc(2, "--------------------------------------");
}

Event::Attach(eventPluginsLoaded, onPluginsLoaded);
