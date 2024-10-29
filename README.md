# JMM Video Uploader

This project aims to have a recurring background job that monitores a folder every X minutes using Hangfire and when it finds video files, uploads them to the configured platform (YouTube, TikTok, etc) creating a title and a description using the OpenAI Api based on the file name.

After the upload has finished, it should move the files to the appropriate folder "uploaded-files" in case of success or "error-files" in case of failure. These will be defined in the appSettings, so it's configurable.
