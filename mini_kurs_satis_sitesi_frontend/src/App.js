import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import CourseList from "./components/CourseList";
import CourseDetail from "./components/CourseDetail";
import Cart from "./components/Cart";
import Profile from "./components/Profile";
import CreateCourse from './components/CreateCourse';
import EditCourse from './components/EditCourse';
import InstructorRoute from './components/InstructorRoute';
import { AppProvider } from "./contexts/AppContext";
import { UIProvider } from "./contexts/UIContext";
import { CartProvider } from "./contexts/CartContext";
import { CourseProvider } from "./contexts/CourseContext";

function App() {
  return (
    <Router>
      <UIProvider>
        <AppProvider>
          <CourseProvider>
            <CartProvider>
              <div className="App">
                <Navbar />
                <Routes>
                  <Route path="/" element={<CourseList />} />
                  <Route path="/course/:id" element={<CourseDetail />} />
                  <Route path="/cart" element={<Cart />} />
                  <Route path="/profile" element={<Profile />} />
                  <Route 
                    path="/create-course" 
                    element={
                      <InstructorRoute>
                        <CreateCourse />
                      </InstructorRoute>
                    } 
                  />
                  <Route 
                    path="/edit-course/:id" 
                    element={
                      <InstructorRoute>
                        <EditCourse />
                      </InstructorRoute>
                    } 
                  />
                </Routes>
              </div>
            </CartProvider>
          </CourseProvider>
        </AppProvider>
      </UIProvider>
    </Router>
  );
}

export default App;