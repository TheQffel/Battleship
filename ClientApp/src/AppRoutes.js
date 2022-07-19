import MainMenu from "./pages/MainMenu";
import Game from "./pages/Game";

const AppRoutes = 
[
  {
    index: true,
    element: <MainMenu />
  },
  {
    path: '/game',
    element: <Game />
  }
];

export default AppRoutes;
