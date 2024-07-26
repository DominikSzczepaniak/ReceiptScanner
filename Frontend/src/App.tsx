import { BrowserRouter, Routes, Route } from 'react-router-dom';
import MainPage from './Pages/MainPage';
import Login from './Pages/UserPage';
import Sidebar from './components/Sidebar';

function App() {
  return (
    <BrowserRouter>
      <Sidebar>
        <Routes>
          <Route path="/" element={<MainPage />} />
          <Route path="/login" element={<Login />} />
        </Routes>
      </Sidebar>
    </BrowserRouter>
  )
}

export default App