FROM mono:5.2
RUN mkdir clase
WORKDIR /clase
RUN apt install make
CMD ["/bin/bash"]
