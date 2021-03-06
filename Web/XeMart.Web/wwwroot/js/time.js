﻿// We attach it to the jQuery object in order to easily invoke it in chat.js and chat-admin.js
jQuery.parseTime = function parseTime() {
    $("time").each(function (i, e) {
        const dateTimeValue = $(e).attr("datetime");
        if (!dateTimeValue) {
            return;
        }

        const time = moment.utc(dateTimeValue, "DD-MMM-YYYY HH:mm").local();
        $(e).html(time.format("DD-MMM-YYYY HH:mm"));
        $(e).attr("title", $(e).attr("datetime"));
    });
}

$(jQuery.parseTime());