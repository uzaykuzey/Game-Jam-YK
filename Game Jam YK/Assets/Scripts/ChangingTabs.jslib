mergeInto(LibraryManager.library, {
    OpenURLInSameTab: function(url) {
        window.location.href = UTF8ToString(url);
    }
});