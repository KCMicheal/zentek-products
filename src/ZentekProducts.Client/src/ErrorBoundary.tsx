import type { ReactNode } from 'react';
import { Component } from 'react';

type Props = {
  children: ReactNode;
};

type State = {
  error: Error | null;
};

export class ErrorBoundary extends Component<Props, State> {
  state: State = { error: null };

  static getDerivedStateFromError(error: Error): State {
    return { error };
  }

  render() {
    if (this.state.error) {
      return (
        <div style={{ padding: 24, fontFamily: 'system-ui, Segoe UI, Roboto, sans-serif' }}>
          <h1 style={{ margin: '0 0 12px' }}>App crashed while rendering</h1>
          <p style={{ margin: '0 0 12px' }}>
            Check the console for details. The error message is:
          </p>
          <pre
            style={{
              whiteSpace: 'pre-wrap',
              background: '#111827',
              color: '#f9fafb',
              padding: 12,
              borderRadius: 8,
              overflowX: 'auto',
            }}
          >
            {this.state.error.message}
          </pre>
        </div>
      );
    }

    return this.props.children;
  }
}

