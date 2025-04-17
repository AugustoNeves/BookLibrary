FROM node:20-alpine3.20
WORKDIR /app
COPY frontend .
RUN npm install 
RUN npm run build
EXPOSE 5173
CMD ["npm", "run", "dev", "--", "--host", "--strictPort" ]