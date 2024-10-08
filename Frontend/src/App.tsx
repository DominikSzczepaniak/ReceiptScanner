import { BrowserRouter, Routes, Route } from 'react-router-dom';
import MainPage from './Pages/MainPage';
import Login from './Pages/UserPage';
import AddReceipt from './Pages/AddReceipt';
import Sidebar from './components/Sidebar';
import MyReceipts from "@/Pages/MyReceipts.tsx";
import ReceiptInfo from "@/Pages/ReceiptInfo.tsx";

function App() {
  return (
    <BrowserRouter>
      <Sidebar>
        <Routes>
          <Route path="/" element={<MainPage />} />
          <Route path="/login" element={<Login />} />
          <Route path="/add" element={<AddReceipt />} />
          <Route path="/receipts" element={<MyReceipts />} />
          <Route path="/receipt/:id" element={<ReceiptInfo />} />
        </Routes>
      </Sidebar>
    </BrowserRouter>
  )
}

export default App