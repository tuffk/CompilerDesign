FROM mono:5.2
RUN mkdir clase
WORKDIR /clase
ENV FILES=""
COPY sicharp.sh sicharp.sh
CMD ["/bin/bash","sicharp.sh"]
