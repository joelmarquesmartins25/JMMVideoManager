# JMM Video Uploader

This project aims to have a recurring background job that monitores a folder every X minutes using Hangfire and when it finds video files, uploads them to the configured platform (YouTube, TikTok, etc) creating a title and a description using the OpenAI Api based on the file name.
