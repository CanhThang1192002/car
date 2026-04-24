import type { ReactNode } from 'react'
import { NavLink, useNavigate } from 'react-router-dom'
import { Bell, CalendarDays, Gift, Image, LayoutDashboard, LogOut, Newspaper, Package, ShoppingBag, Star, Store, Users } from 'lucide-react'

import { clearAuth } from '../../app/auth/authStore'

function NavItem({
  to,
  label,
  icon,
  end = true,
}: {
  to: string
  label: string
  icon: ReactNode
  end?: boolean
}) {
  return (
    <NavLink
      to={to}
      className={({ isActive }) =>
        [
          'group flex items-center gap-3 rounded-xl px-3 py-2.5 text-sm transition-colors',
          'focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-indigo-500/30',
          isActive
            ? [
                'bg-white text-indigo-700 shadow-sm ring-1 ring-indigo-100',
                'dark:bg-zinc-950 dark:text-indigo-200 dark:ring-indigo-500/10',
              ].join(' ')
            : [
                'text-slate-700 hover:bg-white/70 hover:shadow-sm hover:ring-1 hover:ring-slate-200/70',
                'dark:text-zinc-200 dark:hover:bg-zinc-950/60 dark:hover:ring-zinc-800/70',
              ].join(' '),
        ].join(' ')
      }
      end={end}
    >
      {({ isActive }) => (
        <>
          <span
            className={[
              'transition-colors',
              isActive
                ? 'text-indigo-600 dark:text-indigo-300'
                : 'text-slate-400 group-hover:text-slate-500 dark:text-zinc-400 dark:group-hover:text-zinc-300',
            ].join(' ')}
          >
            {icon}
          </span>
          <span className="truncate font-medium">{label}</span>
        </>
      )}
    </NavLink>
  )
}

export function AdminSidebar({ onNavigate }: { onNavigate?: () => void }) {
  const navigate = useNavigate()

  return (
    <div className="flex h-full flex-col">
      <div className="px-3 pt-3">
        <div className="mb-3 flex items-center gap-3">
          <div className="h-px flex-1 bg-slate-200/70 dark:bg-zinc-800/70" />
          <div className="text-xs font-semibold uppercase tracking-wide text-slate-400 dark:text-zinc-500">
            Menu
          </div>
          <div className="h-px flex-1 bg-slate-200/70 dark:bg-zinc-800/70" />
        </div>
      </div>

      <div className="px-3 pb-3" onClick={onNavigate}>
        <div className="space-y-2.5">
          <NavItem to="/" label="Tổng quan" icon={<LayoutDashboard size={16} />} />
          <NavItem to="/banners" label="Banner" icon={<Image size={16} />} />
          <NavItem to="/cars" label="Xe" icon={<Package size={16} />} end={false} />
        </div>

        <div className="mt-2 space-y-2.5">
          <NavItem to="/showrooms" label="Showroom" icon={<Store size={16} />} />
          <NavItem to="/orders" label="Đơn hàng" icon={<ShoppingBag size={16} />} />
          <NavItem to="/bookings" label="Đặt lịch" icon={<CalendarDays size={16} />} />
          <NavItem to="/users" label="Người dùng" icon={<Users size={16} />} />
          <NavItem to="/inventories" label="Tồn kho" icon={<Store size={16} />} />
          <NavItem to="/articles" label="Bài viết" icon={<Newspaper size={16} />} />
          <NavItem to="/promotions" label="Khuyến mãi" icon={<Gift size={16} />} />
          <NavItem to="/notifications" label="Thông báo" icon={<Bell size={16} />} />
          <NavItem to="/reviews" label="Đánh giá" icon={<Star size={16} />} />
        </div>
      </div>

      <div className="mt-auto border-t border-slate-200/70 px-4 py-4 dark:border-zinc-800/70">
        <button
          type="button"
          className={[
            'flex w-full items-center justify-center gap-2 rounded-xl px-3 py-2 text-sm font-medium transition-colors',
            'border border-slate-200 bg-white text-slate-700 hover:bg-slate-50',
            'dark:border-zinc-800 dark:bg-zinc-950 dark:text-zinc-200 dark:hover:bg-zinc-900',
            'focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-indigo-500/30',
          ].join(' ')}
          onClick={() => {
                clearAuth()
                navigate('/login')
          }}
        >
          <LogOut size={16} />
          Đăng xuất
        </button>
      </div>
    </div>
  )
}

