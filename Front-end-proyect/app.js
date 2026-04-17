import { projectRoot } from "./path.js";
import express from "express";
import path from "path";

const app = express();

app.use(express.static(projectRoot));

app.get("/", (req, res) => {
  res.sendFile(path.join(projectRoot, "Assets", "view", "index.html"));
});

const PORT = process.env.PORT || 8000;
app.listen(PORT, () => console.log("Servidor corriendo in http://localhost:" + PORT));