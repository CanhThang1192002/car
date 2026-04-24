import type { RouteObject } from 'react-router-dom'

import { AdminLayout } from '../../layouts/AdminLayout'
import { DashboardPage } from '../../pages/dashboard/DashboardPage'
import { LoginPage } from '../../pages/login/LoginPage'
import { CarsNewPage } from '../../pages/cars/CarsNewPage'
import { NotFoundPage } from '../../pages/NotFoundPage'
import { BannersPage } from '../../pages/banners/BannersPage'
import { CarsListPage } from '../../pages/cars/CarsListPage'
import { CarsEditPage } from '../../pages/cars/CarsEditPage'
import { ShowroomsPage } from '../../pages/showrooms/ShowroomsPage'
import { OrdersPage } from '../../pages/orders/OrdersPage'
import { BookingsPage } from '../../pages/bookings/BookingsPage'
import { UsersPage } from '../../pages/users/UsersPage'
import { ArticlesPage } from '../../pages/articles/ArticlesPage'
import { PromotionsPage } from '../../pages/promotions/PromotionsPage'
import { NotificationsPage } from '../../pages/notifications/NotificationsPage'
import { InventoriesPage } from '../../pages/inventories/InventoriesPage'
import { ReviewsPage } from '../../pages/reviews/ReviewsPage'

export const routes: RouteObject[] = [
  {
    path: '/login',
    element: <LoginPage />,
  },
  {
    element: <AdminLayout />,
    children: [
      { path: '/', element: <DashboardPage /> },
      { path: '/banners', element: <BannersPage /> },
      { path: '/cars', element: <CarsListPage /> },
      { path: '/cars/new', element: <CarsNewPage /> },
      { path: '/cars/:id/edit', element: <CarsEditPage /> },
      { path: '/showrooms', element: <ShowroomsPage /> },
      { path: '/orders', element: <OrdersPage /> },
      { path: '/bookings', element: <BookingsPage /> },
      { path: '/users', element: <UsersPage /> },
      { path: '/articles', element: <ArticlesPage /> },
      { path: '/promotions', element: <PromotionsPage /> },
      { path: '/notifications', element: <NotificationsPage /> },
      { path: '/inventories', element: <InventoriesPage /> },
      { path: '/reviews', element: <ReviewsPage /> },
      { path: '*', element: <NotFoundPage /> },
    ],
  },
]

