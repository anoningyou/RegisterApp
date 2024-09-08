import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: "register",
    loadComponent: () => import("./components/pages/register/register.component").then(m => m.RegisterComponent)
  },
  {
    path: "",
    loadComponent: () => import("./components/pages/hello/hello.component").then(m => m.HelloComponent),
    pathMatch: "full"
  }

];
