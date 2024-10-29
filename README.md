# JMM Video Publisher

This project aims to have a recurring background job that monitores a folder every X minutes using Hangfire and when it finds video files, publishes them to the configured platform (YouTube, TikTok, etc) creating a title and a description using the OpenAI Api based on the file name.

After publishing it, it will move the files to the appropriate folder "published" in case of success or "error" in case of failure.

These can be defined in the appSettings, using the following properties:
- VideosToPublishFolder : Defines the folder that is going to be monitored to get the video files;
- PublishedVideosFolder : Defines the folder to move the video file in case of success;
- ErrorVideosFolder : Defines the folder to move the video file in case of failure;
- TimeIntervalMinutes: Defines the interval in minutes for the verification of the folder.