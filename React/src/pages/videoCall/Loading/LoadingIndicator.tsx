// LoadingIndicator.tsx
import './LoadingIndicator.css';

export const LoadingIndicator = () => {
  return (
    <div className="loading-container">
      <div className="loading-spinner">
        <div className="spinner-ring"></div>
        <div className="spinner-ring"></div>
        <div className="spinner-ring"></div>
        <div className="spinner-ring"></div>
      </div>
      <p className="loading-text">Loading...</p>
    </div>
  );
};

// För att använda som inline/sparknapp
export const SmallLoadingIndicator = () => {
  return (
    <div className="small-loading-spinner">
      <div className="spinner-dot"></div>
      <div className="spinner-dot"></div>
      <div className="spinner-dot"></div>
    </div>
  );
};