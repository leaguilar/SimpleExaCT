package main

import (
  "log"
  "net/http"
)

func main() {
  fs := http.FileServer(http.Dir("./ZollMTURK"))
  http.Handle("/", fs)

  log.Println("Listening on :8080...")
  log.Println("http://localhost:8080/index.html?ExpID=AABBCC&group=1")
  err := http.ListenAndServe(":8080", nil)
  if err != nil {
    log.Fatal(err)
  }
}
