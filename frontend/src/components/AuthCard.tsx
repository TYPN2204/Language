import type { PropsWithChildren } from 'react';

interface AuthCardProps extends PropsWithChildren {
  title: string;
  subtitle?: string;
}

export function AuthCard({ title, subtitle, children }: AuthCardProps) {
  return (
    <div className="auth-card">
      <h1>{title}</h1>
      {subtitle && <p className="auth-card__subtitle">{subtitle}</p>}
      {children}
    </div>
  );
}

